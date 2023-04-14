import { Component, OnInit } from '@angular/core';
import { ApiHelperService } from '../services/api-helper.service';
import { getID, getName } from '../services/storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-session',
  templateUrl: './session.component.html',
  styleUrls: ['./session.component.scss']
})
export class SessionComponent implements OnInit {

  // board
  boardPlayers: Player[] = []
  showCards: boolean = false;

  // user stories
  currentUserStory: string = "Loading...";
  tabCards: string[] = ['1', '2', '3', '5', '8', '13', '1000'];

  // player deck
  selectedCard: string | undefined;
  disabled: boolean = false;
  hasVoted: boolean = false;


  constructor(private api: ApiHelperService, private router: Router) { }

  ngOnInit(): void {
    this.refreshBoard();
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

    //refresh player  & player cards every 1 second
    setTimeout(() => { this.refreshBoard(); }, 1000);
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
      let ids = Object.keys(session.usersNotes);
      let notes: string[] = [];
      for (const id of ids) {
        notes.push(session.usersNotes[id]);
      }
      this.showCards = true;
      // update player card for each player
      this.boardPlayers.forEach((player) => {
        session.users.forEach((user: any) => {
          if (user.name == player.name) {
            if (ids.includes(player.id.toString())) {
              player.card = notes[user.id - 1];
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
      this.router.navigateByUrl("/end");
    }

    this.currentUserStory = session.currentUserStory.description;
    console.log(session.currrentUserStory);
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

    if (this.selectedCard == "∞") { this.selectedCard = ""+Infinity; } // convert infinity to string number
    
    this.api.post({
      endpoint: '/Session/voteCurrentUserStory/'+getID()+'/'+this.selectedCard
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
