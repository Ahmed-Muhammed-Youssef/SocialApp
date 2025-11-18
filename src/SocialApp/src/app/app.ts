import { Component, inject, signal } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { GlobalLoader } from './shared/global-loader/global-loader';
import {MatButtonModule} from '@angular/material/button';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterModule, GlobalLoader, MatButtonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('SocialApp');
  private matIconRegistry: MatIconRegistry = inject(MatIconRegistry);
  private domSanitizer: DomSanitizer = inject(DomSanitizer);
  constructor()
  {
    let url = this.domSanitizer.bypassSecurityTrustResourceUrl('svgs/google-icon-logo.svg');
    this.matIconRegistry.addSvgIcon("sign-in-with-google", url);
  }
}
