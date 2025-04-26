import { Component } from '@angular/core';
import { FrameComponent } from '../../components/frame/frame.component';
import { AutoresComponent } from './autores/autores.component';
import { NgIf } from '@angular/common';
import { AssuntosComponent } from './assuntos/assuntos.component';
import { LivrosComponent } from './livros/livros.component';


@Component({
  selector: 'app-gerenciamento',
  templateUrl: './gerenciamento.component.html', 
  imports: [FrameComponent, NgIf, AutoresComponent, AssuntosComponent,LivrosComponent],  
})
export class GerenciamentoComponent {
  showModal = false;
  tab: string = 'autores';

  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }
}
