import { Component, OnInit } from '@angular/core';
import { ApiHelperService } from '../services/api-helper.service';
import { getID, getName } from '../services/storage.service';
import { ActivatedRoute, Router } from '@angular/router';
import { trigger, style, animate, transition } from '@angular/animations';
import { SocketService } from '../services/socket.service';



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
  currentUserStory: string = "Loading...";
  tabCards: string[] = ['☕', '1', '2', '3', '5', '8', '13', '∞'];

  // player deck
  selectedCard: string | undefined;


  disabled: boolean = false;
  hasVoted: boolean = false;


  constructor(private api: ApiHelperService, private socket: SocketService, private router: Router, private route: ActivatedRoute) {
    this.gameId = "";
    this.route.params.subscribe(params => {
      this.gameId = params['id'];
    });
  }

  ngOnInit(): void {
    this.refreshBoard();
    this.onMessage();
  }

  //
  onMessage(): void {
    this.socket.onMessage().subscribe((message: any) => {
      console.log("Message received");
      console.log(message);
      this.refreshBoardPlayers(message);
      this.refreshUserStory(message);
      this.refreshPlayerDeck(message);
    });
  }

  refreshBoard() {
    this.api.get({ endpoint: '/Session' }).then((response) => {
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
      if (user.name != undefined && user.name != getName()) {
        this.boardPlayers.push(new Player(user.name, user.id));
      }
      if (user.Name != undefined && user.Name != getName()) {
        this.boardPlayers.push(new Player(user.Name, user.Id));
      }
    });

    // if the session is in discussing state, show the cards
    if (session.state == "discussing") {
      let ids = Object.keys(session.usersNotes);
      let notes: string[] = [];
      for (const id of ids) {
        notes.push(session.usersNotes[id]);
      }
      this.showCards = true;
      // update player card for each player
      this.boardPlayers.forEach((player) => {
        session.users.forEach((user: any) => {
          if (user.Name == player.name) {
            if (ids.includes(player.id.toString())) {
              let cardToAdd = notes[user.Id - 1];
              if (cardToAdd == "1000") { cardToAdd = "∞"; } // convert infinity to string number
              if (cardToAdd == "0") { cardToAdd = "☕"; } // convert coffee to string
              player.card = cardToAdd;
            }
          }
        });
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
    }
    this.currentUserStory = session.currentUserStory.description;
  }

  refreshPlayerDeck(session: any) {
    if (session.state == "voting" && !this.hasVoted) {
      this.disabled = false;
    }
    if (session.state == "discussing") {
      this.disabled = true;
      this.hasVoted = false;
    }
  }


  // Send the selected card to the server
  validate(): void {
    this.disabled = true;

    let cardToSend = this.selectedCard;
    if (cardToSend == "∞") { cardToSend = "1000"; } // convert infinity to string number
    if (cardToSend == "☕") { cardToSend = "0"; } // convert coffee to string

    this.api.post({
      endpoint: '/Session/voteCurrentUserStory/' + getID() + '/' + cardToSend
    }).then((response) => {
      console.log("Vote sent");
      console.log(response);
      this.hasVoted = true;
    }).catch((error) => {
      console.log("error while sending vote");
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
