import { NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { AutorService } from '../../../../services/AutorService';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-autores',
  templateUrl: './autores.component.html',
  styleUrls: ['./autores.component.css'],
  imports: [NgFor, FormsModule, NgIf],
})
export class AutoresComponent implements OnInit {
  novo: string = '';
  data: any[] = [];
  editandoIndex: number | null = null;

  constructor(
    private autorService: AutorService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.buscar();
  }

  buscar() {
    this.autorService.list().subscribe({
      next: (res: any[]) => {
        this.data = res;
        this.editandoIndex = null;
      },
      error: () => this.toastr.error('Ocorreu um erro. Tente novamente.')
    });
  }

  adicionar() {
    if (!this.novo.trim()) return;
    this.autorService.create({ nome: this.novo }).subscribe({
      next: () => {
        this.toastr.success('Operação realizada com sucesso!');
        this.novo = '';
        this.buscar();
      },
      error: () => this.toastr.error('Ocorreu um erro. Tente novamente.')
    });
  }

  editar(i: number) {
    this.editandoIndex = i;
  }

  salvar(i: number) {
    const autor = this.data[i];
    if (autor.codAu) {
      this.autorService.update(autor.codAu, { nome: autor.nome }).subscribe({
        next: () => {
          this.toastr.success('Operação realizada com sucesso!');
          this.buscar();
        },
        error: () => this.toastr.error('Ocorreu um erro. Tente novamente.')
      });
    }
  }

  remover(i: number) {
    const autor = this.data[i];
    if (autor.codAu) {
      this.autorService.delete(autor.codAu).subscribe({
        next: () => {
          this.toastr.success('Operação realizada com sucesso!');
          this.buscar();
        },
        error: () => this.toastr.error('Ocorreu um erro. Tente novamente.')
      });
    }
  }

  cancelar() {
    this.buscar();
  }
}
