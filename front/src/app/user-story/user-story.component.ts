import { Component } from '@angular/core';
import { ApiHelperService } from '../services/api-helper.service';

@Component({
  selector: 'app-user-story',
  templateUrl: './user-story.component.html',
  styleUrls: ['./user-story.component.scss']
})
export class UserStoryComponent {

  userStories: number[] = [];
  currrentUserStory: string = "";

  constructor(private api: ApiHelperService) {
    this.api.get({ endpoint: '/Session/UserStory/all' }).then((response) => {
      console.log("User story found");
      console.log(response);
      // for each element in response, add id in userStories
      response.forEach((element: { id: number; }) => {
        this.userStories.push(element.id);
      }
      );
    }).catch((error) => {
      console.log(error);
      console.log("User stories not found");
    });
  }
}