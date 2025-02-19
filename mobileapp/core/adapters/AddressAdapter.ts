import { IAddressService } from '../ports/IAddressService';

export interface IAddressAdapter {
	search: (data: any) => Promise<{
		logradouro: string,
		bairro: string,
		cidade: string,
		estado: string,
	}>,
	estados: () => Promise<any[]>
}

export class AddressAdapter implements IAddressAdapter {
	private service: IAddressService;

	constructor(service: IAddressService) {
		this.service = service;
	}

	async search(cep: string): Promise<{
		logradouro: string,
		bairro: string,
		cidade: string,
		estado: string,
	}> {
		try {
			const response = await this.service.search(cep);

			const data = {
				logradouro: response.data.logradouro ?? '',
				bairro: response.data.bairro ?? '',
				cidade: response.data.localidade ?? '',
				estado: response.data.uf ?? '',
			};

			return data;
		} catch (error) {
			console.error(error);
			throw error;
		}
	}

	async estados() {
		try {
			const response = await this.service.estados();
			return response.data;
		} catch (error) {
			console.error(error);
			throw error;
		}
	}
}