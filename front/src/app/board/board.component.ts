import { Component, OnInit } from '@angular/core';
import { ApiHelperService } from '../services/api-helper.service';
import { getName } from '../services/storage.service';

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
      this.players = response.users.map((user: any) => {
        if (user.name != getName() && !this.players.includes(user.name)) {
          this.players.push(new Player(user.name));
        }
      });
      if (response.state == "discussing") {
        this.showCards = true;
        // update player card for each player
        this.players.forEach((player) => {
          response.users.forEach((user: any) => {
            if (user.name == player.name) {
              player.card = user.card;
            }
          });
        });
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
  card: string | undefined;

  constructor(name: string) {
    this.name = name;
    this.card = undefined;
  }
}