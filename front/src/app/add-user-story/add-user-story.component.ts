import { Component } from '@angular/core';
import {ApiHelperService} from "../services/api-helper.service";
import {FormControl, Validators} from "@angular/forms";
import {Router} from "@angular/router";

@Component({
  selector: 'app-add-user-story',
  templateUrl: './add-user-story.component.html',
  styleUrls: ['./add-user-story.component.scss']
})
export class AddUserStoryComponent {

  private readonly apiService: ApiHelperService;
  private readonly router: Router;

  inputFormControl: FormControl = new FormControl('', [Validators.required]);
  userStories: UserSory[] = [];

  constructor(apiService: ApiHelperService, router: Router) {
    this.apiService = apiService;
    this.router = router;
    this.refreshUserStories();
  }

  refreshUserStories(): void {
    this.apiService.get({endpoint:'/UserStoryProposition'}).then((response) => {
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
      await this.apiService.post({endpoint:'/UserStoryProposition', data:{"description":us}});
    }
    catch (e) {
      console.log("error when posting users story");
    }
  }

  goToWaitingRoom(): void {
    this.router.navigateByUrl("/waiting-room");
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
