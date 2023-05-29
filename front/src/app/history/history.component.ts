import { Component, OnInit } from '@angular/core';
import { UserStory } from '../session/session.component';
import { Router } from '@angular/router';
import { ApiHelperService } from '../services/api-helper.service';
import { getID } from '../services/storage.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit {
  sessions: Session[] = [];

  constructor(private api: ApiHelperService, private router: Router) { }

  async ngOnInit(): Promise<void> {
    try {
      const response = await this.api.get({ endpoint: '/User/' + getID() + '/sessions' })
      console.log(response);
      for (let session of response) {
        let userStories: UserStory[] = [];
        for (let userStory of session.userStories) {
          let tasks = JSON.parse(userStory.tasks).tasks;
          console.log(tasks);
          userStories.push(new UserStory(userStory.id, userStory.description, tasks, userStory.estimatedCost));
        }
        console.log(userStories);
        this.sessions.push(new Session(session.identifier, userStories));
      }
    } catch (error) {
      console.log(" Error getting previous sessions");
    }
  }

  goToHome() {
    this.router.navigate(['/home']);
  }


}

class Session {
  id: string;
  userStories: UserStory[];

  constructor(id: string, userStories: UserStory[]) {
    this.id = id;
    this.userStories = userStories;
  }
}
