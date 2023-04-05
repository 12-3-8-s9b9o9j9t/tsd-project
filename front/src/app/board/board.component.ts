import { Component, OnInit } from '@angular/core';
import { ApiHelperService } from '../services/api-helper.service';
import { getID, getName } from '../services/storage.service';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss']
})
export class BoardComponent implements OnInit {

  players: Player[] = []; // To be filled by the server
  showCards: boolean = false;

  constructor(private api: ApiHelperService) { }

  ngOnInit(): void {
    this.refreshBoard();
  }

  refreshBoard() {
    //refresh player  & player cards every 1 second
    this.api.get({ endpoint: '/Session' }).then((response) => {

      // add players to the board
      this.players = [];
      response.users.map((user: any) => {
        if (user.name != getName()) {
          this.players.push(new Player(user.name, user.id));
        }
      });

      // if the session is in discussing state, show the cards
      if (response.state == "discussing") {
        let ids = Object.keys(response.usersNotes);
        let notes: string[] = [];
        for (const id of ids) {
          notes.push(response.usersNotes[id]);
        }
        this.showCards = true;
        // update player card for each player
        this.players.forEach((player) => {
          response.users.forEach((user: any) => {
            if (user.name == player.name) {
              if(ids.includes(player.id.toString())) {
                player.card = notes[user.id-1];
              }
            }
          });
        });
        console.log(this.players);
      } else {
        this.showCards = false;
      }
    }
    ).catch((error) => {
      console.log(error);
      console.log("Error getting session");
    }
    );
    setTimeout(() => { this.refreshBoard(); }, 1000);
  }
}

class Player {
  name: string;
  id: number;
  card: string | undefined;

  constructor(name: string, id: number) {
    this.name = name;
    this.id = id;
    this.card = undefined;
  }
}