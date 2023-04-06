import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiHelperService } from '../services/api-helper.service';

@Component({
  selector: 'app-user-story',
  templateUrl: './user-story.component.html',
  styleUrls: ['./user-story.component.scss']
})
export class UserStoryComponent implements OnInit {

  currentUserStory: string = "Loading...";

  constructor(private api: ApiHelperService, private router: Router) {}

  ngOnInit(): void {
    // refresh user story every 1 second

    setInterval(() => { this.refreshUserStory(); }, 1000);
  }

  refreshUserStory() {
    this.api.get({ endpoint: '/Session' }).then((response) => {
      if (response.state === "end") {
        this.router.navigateByUrl("/end");
      }
      
      this.currentUserStory = response.currentUserStory.description;
      console.log(response.currrentUserStory);
    }).catch((error) => {
      console.log(error);
      console.log("User stories not found");
    });
  }
}