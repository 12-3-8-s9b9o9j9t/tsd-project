import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { ApiHelperService } from '../services/api-helper.service';
import { getID, getName } from '../services/storage.service';

@Component({
  selector: 'app-waiting-room',
  templateUrl: './waiting-room.component.html',
  styleUrls: ['./waiting-room.component.scss']
})
export class WaitingRoomComponent implements OnInit {


  displayedColumns: string[] = ['name', 'status'];
  player: Player;
  currentPlayers: Player[] = [];

  constructor(private api: ApiHelperService, private router: Router) {
    this.player = new Player(getName());
  }

  ngOnInit(): void {
    this.player = new Player(getName());
    this.refreshCurrentPlayers();

    // post this user in the session
    this.api.post({ endpoint: '/Session/addUser/' + getID() }).then((response) => {
      console.log("User added to session");
    }).catch((error) => {
      console.log(error);
      console.log("Error adding user to session");
    });
  }

  setReady() {
    this.api.post({ endpoint: '/Session/start/' + getID() }).then((response) => {
      
      console.log(response);
      console.log("User ready");
    }).catch((error) => {
      console.log(error);
      console.log("Error sending ready");
    });

    this.player.isPlayerReady = true;
  }

  setNotReady() {
    // this.api.post({ endpoint: '/Session/stop/' + getID() }).then((response) => {
    //   console.log("User not ready");
    // }).catch((error) => {
    //   console.log(error);
    //   console.log("Error sending not ready");
    // });

    this.player.isPlayerReady = false;
  }

  refreshCurrentPlayers() {
    // refresh every 1 second

    // Get players in the session
    this.api.get({ endpoint: '/Session' }).then((response) => {
      this.currentPlayers = []
      response.users.forEach((user: any) => {
        this.currentPlayers.push(new Player(user.name));
      });
      console.log(this.currentPlayers);
      // update user status for each player

      // if session state is "voting", redirect to session page
      if (response.state == "voting") {
        this.router.navigateByUrl('/Session');
      }

    }).catch((error) => {
      console.log(error);
      console.log("Error getting session");
    });


    setTimeout(() => { this.refreshCurrentPlayers(); }, 1000);
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
