import { Component, OnInit } from '@angular/core';
import { getName } from '../services/storage.service';

@Component({
  selector: 'app-waiting-room',
  templateUrl: './waiting-room.component.html',
  styleUrls: ['./waiting-room.component.scss']
})
export class WaitingRoomComponent implements OnInit {


  displayedColumns: string[] = ['name', 'status'];
  player: Player;
  currentPlayers: Player[] = [new Player("John"), new Player("Jane"), new Player("Jack")];

  constructor() { 
    this.player = new Player(getName());
  }

  ngOnInit(): void {
    this.player = new Player(getName());
    this.currentPlayers.push(this.player);
  }

  setReady() {
    this.player.isPlayerReady = true;
  }

  setNotReady() {
    this.player.isPlayerReady = false;
  }

  refreshCurrentPlayers() {
    // refresh every 1 second
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
