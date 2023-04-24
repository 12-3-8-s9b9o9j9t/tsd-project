import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { SessionComponent } from './session/session.component';
import { WaitingRoomComponent } from './waiting-room/waiting-room.component';
import {AddUserStoryComponent} from "./add-user-story/add-user-story.component";
import { EndPageComponent } from './end-page/end-page.component';
import { LoginComponent } from './login/login.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component : LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'session/:id/waiting-room', component: WaitingRoomComponent },
  { path: 'session/:id/game', component: SessionComponent },
  { path: 'session/:id/end', component: EndPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
