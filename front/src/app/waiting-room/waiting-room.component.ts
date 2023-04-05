import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { ApiHelperService } from '../services/api-helper.service';
import { getName } from '../services/storage.service';

@Component({
  selector: 'app-waiting-room',
  templateUrl: './waiting-room.component.html',
  styleUrls: ['./waiting-room.component.scss']
})
export class WaitingRoomComponent implements OnInit {


  displayedColumns: string[] = ['name', 'status'];
  player: Player;
  currentPlayers: Player[] = []//[new Player("John"), new Player("Jane"), new Player("Jack")];

  constructor(private api: ApiHelperService, private router: Router) {
    this.player = new Player(getName());
  }

  ngOnInit(): void {
    this.player = new Player(getName());
    this.currentPlayers.push(this.player);
    this.refreshCurrentPlayers();
  }

  setReady() {
    this.player.isPlayerReady = true;
  }

  setNotReady() {
    this.player.isPlayerReady = false;
  }

  goToAddUS(): void {
    this.router.navigateByUrl("addUserStory");
  }

  refreshCurrentPlayers() {
    // refresh every 1 second

    // if all players are ready, redirect to game page
    if (this.currentPlayers.every(player => player.isPlayerReady)) {
      this.router.navigateByUrl('/session')
    } else {
      // get current players from server

    }
    setTimeout(() => { this.refreshCurrentPlayers(); } ,1000);
  }
}

class Player {
  name: string;
  isPlayerReady: boolean;

  constructor(name: string) {
    this.name = name;
    this.isPlayerReady = false;
  }
}
