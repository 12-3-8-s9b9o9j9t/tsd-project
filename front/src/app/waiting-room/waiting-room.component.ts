import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiHelperService } from '../services/api-helper.service';
import { getID, getName } from '../services/storage.service';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-waiting-room',
  templateUrl: './waiting-room.component.html',
  styleUrls: ['./waiting-room.component.scss']
})
export class WaitingRoomComponent implements OnInit {

  gameId: string;


  displayedColumns: string[] = ['name', 'status'];
  player: Player;
  currentPlayers: Player[] = [];

  inputFormControl: FormControl = new FormControl('');
  userStories: UserSory[] = [];

  constructor(private api: ApiHelperService, private router: Router, private route:ActivatedRoute) {
    this.player = new Player(getName(), getID());
    this.gameId = "";
    this.route.params.subscribe(params => {
      this.gameId = params['id'];
    });
  }

  ngOnInit(): void {
    this.player = new Player(getName(), getID());
    this.refreshCurrentPlayers();
    this.refreshUserStories();

    // post this user in the session
    this.api.post({ endpoint: '/Session/addUser/' + getID() }).then((response) => {
      console.log("User added to session");
    }).catch((error) => {
      console.log(error);
      console.log("Error adding user to session");
    });
  }

  ///////////////////////////////////////////
  ////////// Player display part ////////////
  ///////////////////////////////////////////
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

  goToAddUS(): void {
    this.router.navigateByUrl("addUserStory");
  }

  refreshCurrentPlayers() {
    // refresh every 1 second

    // Get players in the session
    this.api.get({ endpoint: '/Session' }).then((response) => {
      this.currentPlayers = []
      response.users.forEach((user: any) => {
        this.currentPlayers.push(new Player(user.name, user.id));
      });
      console.log(this.currentPlayers);
      // update user status for each player
      let ids = Object.keys(response.usersNotes);
      let notes: string[] = [];
      for (const id of ids) {
        notes.push(response.usersNotes[id].toString());
      }
      console.log(notes);
      this.currentPlayers.forEach((player) => {
        response.users.forEach((user: any) => {
          if (user.name == player.name) {
            if (ids.includes(this.player.id.toString())) {
              if (notes[player.id - 1] == "true") {
                player.isPlayerReady = true;
              } else {
                player.isPlayerReady = false;
              }
            }
          }
        });
      });
      console.log(this.currentPlayers);

      // if session state is "voting", redirect to session page
      if (response.state == "voting") {
        this.router.navigate(['/session', this.gameId, 'game'])
      }

    }).catch((error) => {
      console.log(error);
      console.log("Error getting session");
    });


    setTimeout(() => { this.refreshCurrentPlayers(); }, 1000);
  }


  /////////////////////////////////////////////////////////
  ////////////////// Add user story part //////////////////
  /////////////////////////////////////////////////////////
  refreshUserStories(): void {
    this.api.get({endpoint:'/UserStoryProposition'}).then((response) => {
      // for each user story, create a new UserStory object and add it to the list
      this.userStories = [];
      response.map((us: any) => {
        this.userStories.push(new UserSory(us.description, us.id));
      })
    }).catch((error) => {
      console.log(error);
      console.log("Error getting user stories");
    });

    //refresh user stories every 1 second
    setTimeout(() => { this.refreshUserStories(); }, 1000);
  }

  async postUserStory(): Promise<void> {
    const us: string = this.inputFormControl.value;
    try {
      await this.api.post({endpoint:'/UserStoryProposition', data:{"description":us}});
      this.inputFormControl.setValue("");
    }
    catch (e) {
      console.log("error when posting users story");
    }
  }
}

class Player {
  name: string;
  id: number;
  isPlayerReady: boolean;

  constructor(name: string, id: number) {
    this.name = name;
    this.id = id;
    this.isPlayerReady = false;
  }
}

class UserSory {
  description: string;
  id: number;

  constructor(description: string, id: number) {
    this.description = description;
    this.id = id;
  }
}
