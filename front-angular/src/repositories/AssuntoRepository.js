const rota = `assunto`;
export default {
  async list() {
    const resp = await axios.get(rota);
    return resp.data;
  },
  async get(id) {
    const resp = await axios.get(`${rota}/${id}`);
    return resp.data;
  },
  async create(data) {
    const resp = await axios.post(rota, data);
    return resp.data;
  },
  async update(id, data) {
    const resp = await axios.put(`${rota}/${id}`, data);
    return resp.data;
  },
  async delete(id) {
    const resp = await axios.delete(`${rota}/${id}`);
    return resp.data;
  }
};
