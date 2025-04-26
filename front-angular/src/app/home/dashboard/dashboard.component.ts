import { Component, OnInit } from '@angular/core';
import { LivroService } from '../../../services/LivroService';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit {
  data: any;
  constructor(private livroService: LivroService) {}

  ngOnInit(): void {
    this.livroService.getRelatorio().subscribe((response) => {
      this.data = response[0]; 
    });
  }
}
