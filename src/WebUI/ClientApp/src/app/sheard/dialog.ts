// import { Component, OnInit } from '@angular/core';

// @Component({
//   selector: 'app-reactions-dialog',
//   templateUrl: './reactions-dialog.component.html',
//   styleUrls: ['./reactions-dialog.component.css']
// })
// export class ReactionsDialogComponent implements OnInit {

//   reactions = [
//     {
//       name: 'John Doe',
//       reaction: 'Like'
//     },
//     {
//       name: 'Jane Doe',
//       reaction: 'Love'
//     },
//     {
//       name: 'Peter Smith',
//       reaction: 'Haha'
//     }
//   ];

//   constructor() {}

//   ngOnInit() {}

//   openDialog() {
//     const dialogRef = window.open('https://example.com/reactions', '_blank');
//     dialogRef.addEventListener('load', () => {
//       dialogRef.document.getElementById('reactions').innerHTML = this.reactions.map(reaction => `
//         <li>${reaction.name}: ${reaction.reaction}</li>
//       `).join('');
//     });
//   }

// }