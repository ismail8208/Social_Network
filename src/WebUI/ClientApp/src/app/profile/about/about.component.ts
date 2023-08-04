import { Component, Input, OnChanges, OnInit } from '@angular/core';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnChanges {

  @Input() content?: string;
  displayedContent?: string;
  isTruncated: boolean = true;
  ngOnChanges() {
    this.truncateText();
  }

  truncateText() {
    if (this.content && this.content.length > 270) {
      this.displayedContent = this.content.substr(0, 270) + '...';
      this.isTruncated = true;
    } else {
      this.displayedContent = this.content;
      this.isTruncated = false;
    }
  }

  showFullContent() {
    this.displayedContent = this.content;
    this.isTruncated = false;
  }

  showTruncatedContent() {
    this.displayedContent = this.content!.substr(0, 270) + '...';
    this.isTruncated = true;
  }



}
