import { Injectable } from '@angular/core';
import { AssuntoRepository } from '../repositories/AssuntoRepository';
import { RepositoryType } from '../types/RepositoryType';

@Injectable({
  providedIn: 'root',
})
export class AssuntoService implements RepositoryType {
  constructor(private repository: AssuntoRepository) {}
  
  list() {
    return this.repository.list();
  }
  get(id: any) {
    return this.repository.get(id);
  }
  create(data: any) {
    return this.repository.create(data);
  }
  update(id: any, data: any) {
    return this.repository.update(id, data);
  }
  delete(id: any) {
    return this.repository.delete(id);
  } 
}
