import repository from '../repositories/LivroRepository.js';

export default {
  async getAll() {
    return await repository.list();
  },
  async getById(id) {
    return await repository.get(id);
  },
  async save(project) {
    if (project.id) {
      return await repository.update(project.id, project);
    }
    return await repository.create(project);
  },
  async delete(id) {
    return await repository.delete(id);
  }
};
