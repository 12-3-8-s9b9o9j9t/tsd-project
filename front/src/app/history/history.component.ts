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
    this.sessions = [
      new Session("AB445", [
        new UserStory(1, 'As a user, I want to be able to add a user story to the session', ["task 1","task 2","task 3"], "5"),
        new UserStory(2, 'As a user, I want to be able to vote on a user story', ["task 1","task 2","task 3"], "8"),
        new UserStory(3, 'As a user, I want to be able to see the results of the vote', ["task 1","task 2","task 3"], "13"),
      ]),
      new Session("AB446", [
        new UserStory(1, 'As a user, I want to be able to add a user story to the session', ["task 1","task 2","task 3"], "5"),
        new UserStory(2, 'As a user, I want to be able to vote on a user story', ["task 1","task 2","task 3"], "8"),
        new UserStory(3, 'As a user, I want to be able to see the results of the vote', ["task 1","task 2","task 3"], "13"),
      ]),
    ];
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
