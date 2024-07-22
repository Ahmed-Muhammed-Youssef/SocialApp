import { Component, input, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {MatCardModule} from '@angular/material/card';

@Component({
  selector: 'app-post-card',
  standalone: true,
  imports: [MatCardModule, MatButtonModule],
  templateUrl: './post-card.component.html',
  styleUrl: './post-card.component.css'
})
export class PostCardComponent {
  @Input() postText: string = '';
  @Input() imageUrl: string | null = null;
  @Input() userProfile: string = 'https://www.vhv.rs/dpng/d/505-5058560_person-placeholder-image-free-hd-png-download.png';
  @Input() userName: string | null = null;
}
