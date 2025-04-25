import { Component } from '@angular/core';
import { FrameComponent } from '../../components/frame/frame.component';
import { NgIf } from '@angular/common';


@Component({
  selector: 'app-gerenciamento',
  templateUrl: './gerenciamento.component.html', 
  imports: [FrameComponent,NgIf],  
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
