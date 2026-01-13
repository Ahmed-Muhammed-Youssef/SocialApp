import { Component, Input, inject, signal, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { UserPicturesService } from '../services/user-pictures';
import { PictureDTO } from '../../auth/models/user-dto';

@Component({
  selector: 'app-user-pictures',
  imports: [CommonModule, MatIconModule, MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './user-pictures.html',
  styleUrl: './user-pictures.css',
})
export class UserPictures {
  @Input() userId: number | null = null;

  private userPicturesService = inject(UserPicturesService);

  pictures = signal<PictureDTO[]>([]);
  isCarouselOpen = signal(false);
  currentImageIndex = signal(0);
  isLoading = signal(false);
  error = signal<string | null>(null);
  isSettingProfilePicture = signal(false);
  profilePictureSuccess = signal(false);
  touchStartX = 0;
  touchStartY = 0;
  
  ngOnInit() {
    if (this.userId) {
      this.loadPictures();
    }
  }

  loadPictures() {
    this.isLoading.set(true);
    this.error.set(null);
    this.userPicturesService.GetUserPictures().subscribe({
      next: (res) => {
        this.pictures.set(res);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load pictures', err);
        this.error.set('Failed to load pictures');
        this.isLoading.set(false);
      },
    });
  }

  openCarousel() {
    if (this.pictures().length > 0) {
      this.isCarouselOpen.set(true);
      this.currentImageIndex.set(0);
      document.body.style.overflow = 'hidden';
    }
  }

  closeCarousel() {
    this.isCarouselOpen.set(false);
    document.body.style.overflow = 'auto';
  }

  nextImage() {
    const maxIndex = this.pictures().length - 1;
    const nextIndex = this.currentImageIndex() < maxIndex ? this.currentImageIndex() + 1 : 0;
    this.currentImageIndex.set(nextIndex);
  }

  previousImage() {
    const maxIndex = this.pictures().length - 1;
    const prevIndex = this.currentImageIndex() > 0 ? this.currentImageIndex() - 1 : maxIndex;
    this.currentImageIndex.set(prevIndex);
  }

  @HostListener('touchstart', ['$event'])
  onTouchStart(event: TouchEvent) {
    if (!this.isCarouselOpen()) return;
    this.touchStartX = event.touches[0].clientX;
    this.touchStartY = event.touches[0].clientY;
  }

  @HostListener('touchend', ['$event'])
  onTouchEnd(event: TouchEvent) {
    if (!this.isCarouselOpen()) return;
    
    const touchEndX = event.changedTouches[0].clientX;
    const touchEndY = event.changedTouches[0].clientY;
    
    const deltaX = touchEndX - this.touchStartX;
    const deltaY = touchEndY - this.touchStartY;
    
    // Only consider horizontal swipes (not vertical)
    if (Math.abs(deltaX) > Math.abs(deltaY) && Math.abs(deltaX) > 50) {
      if (deltaX > 0) {
        this.previousImage();
      } else {
        this.nextImage();
      }
    }
  }

  @HostListener('window:keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    if (!this.isCarouselOpen()) return;
    
    switch (event.key) {
      case 'ArrowRight':
        event.preventDefault();
        this.nextImage();
        break;
      case 'ArrowLeft':
        event.preventDefault();
        this.previousImage();
        break;
      case 'Escape':
        event.preventDefault();
        this.closeCarousel();
        break;
    }
  }

  get currentImage(): PictureDTO | null {
    const pics = this.pictures();
    return pics.length > 0 ? pics[this.currentImageIndex()] : null;
  }

  get imageProgress(): string {
    const total = this.pictures().length;
    return total > 0 ? `${this.currentImageIndex() + 1} / ${total}` : '0 / 0';
  }

  setCurrentAsProfilePicture() {
    if (!this.currentImage) return;
    
    this.isSettingProfilePicture.set(true);
    this.profilePictureSuccess.set(false);
    
    this.userPicturesService.setProfilePicture(this.currentImage.id).subscribe({
      next: () => {
        this.profilePictureSuccess.set(true);
        this.isSettingProfilePicture.set(false);
        // Reset success message after 2 seconds
        setTimeout(() => this.profilePictureSuccess.set(false), 2000);
      },
      error: (err) => {
        console.error('Failed to set profile picture', err);
        this.error.set('Failed to set profile picture');
        this.isSettingProfilePicture.set(false);
      },
    });
  }
}
