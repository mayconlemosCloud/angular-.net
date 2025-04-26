import { NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { AssuntoService } from '../../../../services/AssuntoService';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-assuntos',
  templateUrl: './assuntos.component.html',
  styleUrls: ['./assuntos.component.css'],
  imports: [NgFor, FormsModule, NgIf],
})
export class AssuntosComponent implements OnInit {
  novo: string = '';
  data: any[] = [];
  editandoIndex: number | null = null;

  constructor(
    private assuntoService: AssuntoService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.buscar();
  }

  buscar() {
    this.assuntoService.list().subscribe({
      next: (res: any[]) => {
        this.data = res;
        this.editandoIndex = null;
      },
      error: () => this.toastr.error('Ocorreu um erro. Tente novamente.')
    });
  }

  adicionar() {
    if (!this.novo.trim()) return;
    this.assuntoService.create({ descricao: this.novo }).subscribe({
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
    const assunto = this.data[i];
    if (assunto.codAs) {
      this.assuntoService.update(assunto.codAs, { descricao: assunto.descricao }).subscribe({
        next: () => {
          this.toastr.success('Operação realizada com sucesso!');
          this.buscar();
        },
        error: () => this.toastr.error('Ocorreu um erro. Tente novamente.')
      });
    }
  }

  remover(i: number) {
    const assunto = this.data[i];
    if (assunto.codAs) {
      this.assuntoService.delete(assunto.codAs).subscribe({
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
