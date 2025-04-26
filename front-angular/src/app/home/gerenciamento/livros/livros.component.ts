import { Component, OnInit } from '@angular/core';
import { LivroService } from '../../../../services/LivroService';
import { AutorService } from '../../../../services/AutorService';
import { AssuntoService } from '../../../../services/AssuntoService';
import { ToastrService } from 'ngx-toastr';
import { NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DecimalPipe, CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-livros',
  templateUrl: './livros.component.html',
  imports: [NgFor, FormsModule, NgIf, CurrencyPipe],
  providers: [CurrencyPipe]
})
export class LivrosComponent implements OnInit {
  novo: any = { titulo: '', editora: '', edicao: 0, anoPublicacao: '', preco: 0, livroAutores: [], livroAssuntos: [] };
  data: any[] = [];
  autores: any[] = [];
  assuntos: any[] = [];
  editandoIndex: number | null = null;
  mostrarModal: boolean = false;
  formaCompra: string = 'online';
  livroSelecionado: any;

  constructor(
    private livroService: LivroService,
    private autorService: AutorService,
    private assuntoService: AssuntoService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.buscar();
    this.carregarAutores();
    this.carregarAssuntos();
  }

  buscar() {
    this.livroService.list().subscribe({
      next: (res: any[]) => {
        this.data = res;
        this.editandoIndex = null;
      },
      error: () => this.toastr.error('Ocorreu um erro. Tente novamente.')
    });
  }

  carregarAutores() {
    this.autorService.list().subscribe({
      next: (res: any[]) => (this.autores = res),
      error: () => this.toastr.error('Erro ao carregar autores.')
    });
  }

  carregarAssuntos() {
    this.assuntoService.list().subscribe({
      next: (res: any[]) => (this.assuntos = res),
      error: () => this.toastr.error('Erro ao carregar assuntos.')
    });
  }

  adicionar() {
    if (!this.novo.titulo.trim()) {
      this.toastr.error('O título é obrigatório.');
      return;
    }
    if (!this.novo.editora.trim()) {
      this.toastr.error('A editora é obrigatória.');
      return;
    }
    if (!this.novo.anoPublicacao.trim()) {
      this.toastr.error('O ano de publicação é obrigatório.');
      return;
    }
    if (this.novo.edicao <= 0) {
      this.toastr.error('A edição deve ser maior que zero.');
      return;
    }

    const novoLivro = { ...this.novo, preco: parseFloat(this.novo.preco.toString()) };

    this.livroService.create(novoLivro).subscribe({
      next: () => {
        this.toastr.success('Operação realizada com sucesso!');
        this.novo = { titulo: '', editora: '', edicao: 0, anoPublicacao: '', preco: 0, livroAutores: [], livroAssuntos: [] };
        this.buscar();
      },
      error: () => this.toastr.error('Ocorreu um erro. Tente novamente.')
    });
  }

  editar(i: number) {
    const livro = this.data[i];
    console.log('Livro selecionado para edição:', livro); // Log para depuração
    if (livro && livro.codl) {
      this.livroService.get(livro.codl).subscribe({
        next: (res: any) => {
          console.log('Dados do livro recebidos:', res); // Log para depuração
          this.data[i] = res;
          this.editandoIndex = i;
        },
        error: (err) => {
          console.error('Erro ao buscar os dados do livro:', err); // Log para depuração
          this.toastr.error('Erro ao buscar os dados do livro.');
        }
      });
    } else {
      console.warn('Livro ou codl inválido:', livro); // Log para depuração
    }
  }

  salvar(i: number) {
    const livro = this.data[i];
    if (livro.codl) {
      const payload = {
        titulo: livro.titulo,
        editora: livro.editora,
        edicao: livro.edicao,
        anoPublicacao: livro.anoPublicacao,
        preco: parseFloat(livro.preco.toString()),
        livroAutores: livro.autores.map((autor: any) => autor.codAu),
        livroAssuntos: livro.assuntos.map((assunto: any) => assunto.codAs),
      };

      this.livroService.update(livro.codl, payload).subscribe({
        next: () => {
          this.toastr.success('Operação realizada com sucesso!');
          this.buscar();
        },
        error: () => this.toastr.error('Ocorreu um erro ao salvar. Tente novamente.')
      });
    }
  }

  remover(i: number) {
    const livro = this.data[i];
    if (livro.codl) {
      this.livroService.delete(livro.codl).subscribe({
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

  toggleAutor(codAu: number, event: Event) {
    const checked = (event.target as HTMLInputElement).checked;
    if (checked) {
      this.novo.livroAutores.push(codAu);
    } else {
      this.novo.livroAutores = this.novo.livroAutores.filter((id: number) => id !== codAu);
    }
  }

  toggleAssunto(codAs: number, event: Event) {
    const checked = (event.target as HTMLInputElement).checked;
    if (checked) {
      this.novo.livroAssuntos.push(codAs);
    } else {
      this.novo.livroAssuntos = this.novo.livroAssuntos.filter((id: number) => id !== codAs);
    }
  }

  formatarPreco(event: Event): void {
    const input = event.target as HTMLInputElement;
    let valor = input.value.replace(/[^\d,]/g, '').replace(',', '.');
    this.novo.preco = parseFloat(valor) || 0;
    input.value = new DecimalPipe('pt-BR').transform(this.novo.preco, '1.2-2') || '';
  }

  comprarLivro(livro: any) {
    this.livroSelecionado = livro;
    this.mostrarModal = true;
  }

  fecharModal() {
    this.mostrarModal = false;
    this.formaCompra = 'online';
  }

  confirmarCompra() {
    if (this.livroSelecionado && this.formaCompra.trim()) {
      const payload = {
        livroCodl: this.livroSelecionado.codl,
        formaDeCompra: this.formaCompra
      };

      this.livroService.postTransaction(payload).subscribe({
        next: () => {
          this.toastr.success('Compra realizada com sucesso!');
          this.fecharModal();
        },
        error: () => this.toastr.error('Erro ao realizar a compra. Tente novamente.')
      });
    } else {
      this.toastr.error('Selecione um livro e uma forma de compra válida.');
    }
  }
}
