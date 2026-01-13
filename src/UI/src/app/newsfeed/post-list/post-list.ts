import { afterRenderEffect, Component, ElementRef, inject, signal, viewChild } from '@angular/core';
import { NewsfeedService } from '../services/newsfeed';
import { PostDTO } from '../models/post-dto';
import { MatIconModule } from '@angular/material/icon';
import { PostItem } from '../post-item/post-item';

@Component({
  selector: 'app-post-list',
  imports: [MatIconModule, PostItem],
  templateUrl: './post-list.html',
  styleUrl: './post-list.css',
})
export class PostList {
  private newsfeedService = inject(NewsfeedService);
  posts = signal<PostDTO[]>([]);
  pageNumber = 1;

  readonly itemsPerPage = 10;
  loading = signal(false);
  hasNext = signal(true);


  // anchor element reference
  scrollAnchor = viewChild<ElementRef>('scrollAnchor');

  constructor() {
    this.loadPosts();

    // attach intersection observer AFTER DOM renders
    afterRenderEffect(() => {
      const anchor = this.scrollAnchor()?.nativeElement;
      if (!anchor) return;

      const observer = new IntersectionObserver(entries => {
        if (entries.some(e => e.isIntersecting)) {
          this.loadPosts();
        }
      });

      observer.observe(anchor);

      // cleanup automatically when component is destroyed
      return () => observer.disconnect();
    });
  }

  private loadPosts() {
    if (this.loading() || !this.hasNext()) return;

    this.loading.set(true);

    this.newsfeedService.getPosts(this.pageNumber, this.itemsPerPage).subscribe({
      next: paged => {
        this.posts.update(cur => [...cur, ...paged.items]);

        this.hasNext.set(paged.hasNext);
        if (paged.hasNext) this.pageNumber++;

        this.loading.set(false);
      },
      error: err => {
        console.error(err);
        this.loading.set(false);
      }
    });
  }
}
