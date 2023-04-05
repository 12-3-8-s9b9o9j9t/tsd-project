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

  constructor(
    apiService: ApiHelperService,
    router: Router
  ) {
    this.apiService = apiService;
    this.router = router;
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
