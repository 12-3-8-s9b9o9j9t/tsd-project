import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { SessionComponent } from './session/session.component';
import { WaitingRoomComponent } from './waiting-room/waiting-room.component';
import {AddUserStoryComponent} from "./add-user-story/add-user-story.component";
import { EndPageComponent } from './end-page/end-page.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },

  { path: 'home', component: HomeComponent },
  { path: 'waiting-room', component: WaitingRoomComponent },
  { path: 'session', component: SessionComponent },
  { path: 'addUserStory', component: AddUserStoryComponent },
  { path: 'end', component: EndPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
