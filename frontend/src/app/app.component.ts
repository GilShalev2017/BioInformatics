import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { BioinformaticsTabsComponent } from './bioinformatics-tabs/bioinformatics-tabs.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [CommonModule, BioinformaticsTabsComponent],
})
export class AppComponent {
  title = 'frontend';
}