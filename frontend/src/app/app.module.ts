import { AgGridModule } from 'ag-grid-angular';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './router/app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { ChartsModule } from 'ng2-charts';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HeaderComponent } from './components/header/header.component';
import { HeroesComponent } from './components/heroes/heroes.component';
import { HomeComponent } from './components/home/home.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { PowersComponent } from './components/powers/powers.component';
import { PowerChartComponent } from './components/charts/power-chart/power-chart.component';
import { HeroChartComponent } from './components/charts/hero-chart/hero-chart.component';
import { MainComponent } from './components/main/main.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { JwtInterceptor } from './helpers/jwt.interceptor';
import { ErrorInterceptor } from './helpers/error.interceptor';
import { MyProfileComponent } from './components/my-profile/my-profile.component';
import { HeroImageRendererComponent } from './components/hero-image-renderer/hero-image-renderer.component';
import { UnauthorizedComponent } from './components/unauthorized/unauthorized.component';
import { DeleteHeroCellComponent } from './components/delete-cell/delete-hero-cell.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { UploadUserComponent } from './components/upload-user/upload-user.component';
import { UploadHeroComponent } from './components/upload-hero/upload-hero.component';
import { ModalModule } from 'ngx-bootstrap/modal';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HeroesComponent,
    HomeComponent,
    PageNotFoundComponent,
    PowersComponent,
    PowerChartComponent,
    HeroChartComponent,
    MainComponent,
    LoginComponent,
    RegisterComponent,
    MyProfileComponent,
    HeroImageRendererComponent,
    UnauthorizedComponent,
    DeleteHeroCellComponent,
    ResetPasswordComponent,
    UploadUserComponent,
    UploadHeroComponent
    ],
  imports: [
    AgGridModule.withComponents([]),
    AppRoutingModule,
    BrowserModule,
    ChartsModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    ModalModule.forRoot()
  ],
  providers: [  
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }],
  bootstrap: [AppComponent],
})
export class AppModule { }
