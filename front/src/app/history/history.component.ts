import { Component, OnInit } from '@angular/core';
import { UserStory } from '../session/session.component';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit {
  sessions: Session[] = [];

  constructor() { }

  ngOnInit(): void {
    // TODO get sessions from backend
  }

}

class Session {
  id: number;
  userStories: UserStory[];

  constructor(id: number, userStories: UserStory[]) {
    this.id = id;
    this.userStories = userStories;
  }
}
