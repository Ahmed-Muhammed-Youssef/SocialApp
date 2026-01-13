import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoaderService {
  loading = signal(false);

  show() { this.loading.set(true); }
  hide() { this.loading.set(false); }
}
