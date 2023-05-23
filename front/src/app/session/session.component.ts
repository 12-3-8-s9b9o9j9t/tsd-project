import { Component, OnInit } from '@angular/core';
import { ApiHelperService } from '../services/api-helper.service';
import { getID, getName, getSessionIdentifier, isOwner } from '../services/storage.service';
import { ActivatedRoute, Router } from '@angular/router';
import { trigger, style, animate, transition } from '@angular/animations';
import { SocketService } from '../services/socket.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';



@Component({
  selector: 'app-session',
  templateUrl: './session.component.html',
  styleUrls: ['./session.component.scss'],
  animations: [
    trigger('fade', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('300ms ease-in', style({ opacity: 1 }))
      ]),
      transition(':leave', [
        animate('300ms ease-in', style({ opacity: 0 }))
      ])
    ])
  ]

})
export class SessionComponent implements OnInit {

  gameId: string;

  // board
  boardPlayers: Player[] = []
  showCards: boolean = false;

  // user stories
  currentUserStory: UserStory = new UserStory(-1, "Loading...",[]);
  taskControl = new FormControl('', [Validators.required]);
  formGroup = new FormGroup({ task: this.taskControl });

  // player deck
  tabCards: string[] = ['☕', '1', '2', '3', '5', '8', '13', '∞'];
  selectedCard: string | undefined;


  disabled: boolean = false;
  hasVoted: boolean = false;
  isOwner: boolean = false;


  constructor(private api: ApiHelperService, private socket: SocketService, private router: Router, private route: ActivatedRoute) {
    this.gameId = "";
    this.route.params.subscribe(params => {
      this.gameId = params['id'];
    });
    this.isOwner = isOwner();
  }

  ngOnInit(): void {
    this.refreshBoard();
    this.onMessage();
  }

  onMessage(): void {
    this.socket.onMessage().subscribe((message: any) => {
      if (message.type == "session") {
        this.refreshBoardPlayers(message.session);
        this.refreshUserStory(message.session);
        this.refreshPlayerDeck(message.session);
      }
    });
  }

  async refreshBoard() {
    await this.api.get({ endpoint: '/Session/' + getSessionIdentifier() }).then((response) => {
      // refresh players, user stories and player deck
      this.refreshBoardPlayers(response);
      this.refreshUserStory(response);
      this.refreshPlayerDeck(response);
    }).catch((error) => {
      console.log(error);
      console.log("Error getting session");
    });
  }

  refreshBoardPlayers(session: any) {
    // add players to the board
    this.boardPlayers = [];
    session.users.map((user: any) => {
      if (user.name != getName()) {
        this.boardPlayers.push(new Player(user.name, user.id));
      }
    });

    // if the session is in discussing state, show the cards
    if (session.state == "discussing") {
      this.showCards = true;
      let ids = Object.keys(session.usersNotes);

      // for every player, get the note and update the card
      this.boardPlayers.forEach((player) => {
        let cardToAdd = session.usersNotes[player.id];
        if (cardToAdd == "1000") { cardToAdd = "∞"; } // convert infinity to string number
        if (cardToAdd == "0") { cardToAdd = "☕"; } // convert coffee to string
        if (cardToAdd == "-1") { cardToAdd = "?"; } // convert null to string
        player.card = cardToAdd;
      });

      console.log(this.boardPlayers);
    } else {
      this.showCards = false;
    }

  }

  refreshUserStory(session: any) {
    if (session.state === "end") {
      this.socket.disconnect();
      this.router.navigate(['/session', this.gameId, 'end'])
    } else {
      if (session.currentUserStory.description != this.currentUserStory.description) {
        this.currentUserStory = new UserStory(session.currentUserStory.id, session.currentUserStory.description, session.currentUserStory.tasks);
      }
      console.log("Avant : "+this.currentUserStory.tasks);
      console.log(session.currentUserStory.tasks);
      this.currentUserStory.tasks = JSON.parse(session.currentUserStory.tasks).tasks;
      console.log("Après : "+this.currentUserStory.tasks);

    }
  }

  refreshPlayerDeck(session: any) {
    if (session.state == "voting" && !this.hasVoted) {
      console.log("refreshPlayerDeck --> disabled: false");
      this.disabled = false;
    }
    if (session.state == "discussing") {
      console.log("refreshPlayerDeck --> disabled: true");
      this.disabled = true;
      this.hasVoted = false;
    }
  }


  // Send the selected card to the server
  async validate(): Promise<void> {
    console.log("validate --> disabled: true");
    this.disabled = true;

    let cardToSend = this.selectedCard;
    if (cardToSend == "∞") { cardToSend = "1000"; } // convert infinity to string number
    if (cardToSend == "☕") { cardToSend = "0"; } // convert coffee to string

    this.hasVoted = true;
    this.api.post({
      endpoint: '/Session/voteCurrentUserStory/' + getID() + '/' + cardToSend + "/" + getSessionIdentifier()
    }).then((response) => {
      console.log("Vote sent");
      //console.log(response);
    }).catch((error) => {
      console.log("error while sending vote");
      console.log(error);
    });

  }


  async addTask() {
    if (this.taskControl.value && this.taskControl.valid) {
      this.currentUserStory.tasks.push(this.taskControl.value);

      await this.api.put({
        endpoint: '/UserStoryProposition/' + this.currentUserStory.id,
        data: {
          description: this.currentUserStory.description,
          tasks: JSON.stringify({tasks: this.currentUserStory.tasks}),
          sessionIdentifier: getSessionIdentifier()
        }
      }).then((response) => {
        console.log("Task added");
        console.log(response);
      }).catch((error) => {
        console.log("error while adding task");
        console.log(error);
      });
        

      this.taskControl.reset();
    }
  }

  async deleteTask(task: string) {
    this.currentUserStory.tasks = this.currentUserStory.tasks.filter((t) => t != task);


    await this.api.put({
      endpoint: '/UserStoryProposition/' + this.currentUserStory.id,
      data: {
        description: this.currentUserStory.description,
        tasks: JSON.stringify({tasks: this.currentUserStory.tasks}),
        sessionIdentifier: getSessionIdentifier()
      }
    }).then((response) => {
      console.log("Task deleted");
      console.log(response);
    }).catch((error) => {
      console.log("error while deleting task");
      console.log(error);
    });
  }

  forceShow() {
    this.api.get({
      endpoint: '/Session/showVotesOfEveryone/' + getSessionIdentifier()
    }).then((response) => {
      console.log("Force show sent");
      //console.log(response);
    }).catch((error) => {
      console.log("error while sending force show");
      console.log(error);
    });
  }
}

class Player {

  name: string;
  id: number;
  card: string | undefined;

  constructor(public _name: string, public _id: number) {
    this.name = _name;
    this.id = _id;
    this.card = undefined;
  }
}

class UserStory {
  id: number;
  description: string;
  tasks: string[];

  constructor(public _id: number, public _description: string, public _tasks: string[]) {
    this.id = _id;
    this.description = _description;
    if (!_tasks){
      this.tasks = _tasks;
    } else {
      this.tasks = []
    }
  }
}