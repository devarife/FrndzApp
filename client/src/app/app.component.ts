import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  http = inject(HttpClient);
  title = 'FrndzApp';
  Users: any;

  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/user/all').subscribe({
      next: (res) => (this.Users = res),
      error: (err) => console.log(err),
      complete: () => console.log('Request complete!'),
    });
  }
}
