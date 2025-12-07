
import { Component, inject } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LoaderService } from '../services/loader';

@Component({
  selector: 'app-global-loader',
  imports: [MatProgressSpinnerModule],
  templateUrl: './global-loader.html',
  styleUrl: './global-loader.css',
})
export class GlobalLoader {
  loader = inject(LoaderService);
}
