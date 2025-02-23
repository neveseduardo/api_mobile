import { useEffect, useState } from 'react';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import Button from '@/components/ui/Button';
import { useRouter } from 'expo-router';
import { IAddress } from '@/services/IAddress';
import { AxiosHttpClient } from '@/services/AxiosHttpClient';
import { useAddressService } from '@/services/AddressService';
import { useAuth } from '@/contexts/AuthContext';
import { FlatList, View } from 'react-native';

const client = AxiosHttpClient;

const AddressScreen = () => {
	const { service } = useAddressService(client);
	const router = useRouter();
	const [loading, setLoading] = useState<boolean>(false);
	const [list, setList] = useState<IAddress[]>([]);
	const { userData } = useAuth();

	useEffect(() => {
		const getAddresses = async () => {
			if (!userData?.id) return;

			try {
				setLoading(true);
				const response = await service.getAddresses(userData.id);
				setList(Array.isArray(response) ? response : []);
			} catch (error) {
				console.error(error);
			} finally {
				setLoading(false);
			}
		};

		getAddresses();
	}, [userData?.id]); // Agora o efeito só roda quando `userData?.id` mudar

	return (
		<ThemedView className="items-center justify-center flex-1 p-5">
			<ThemedView className="flex items-center justify-center w-full gap-4">
				<ThemedText>LISTA DE ENDEREÇOS</ThemedText>

				{loading ? (
					<ThemedText>Carregando...</ThemedText>
				) : (
					<View>
						<FlatList
							data={list}
							keyExtractor={(item) => String(item.id)}
							renderItem={({ item }) => (
								<View style={{ padding: 10, borderBottomWidth: 1, borderColor: '#ccc' }}>
									<ThemedText>{item.logradouro}</ThemedText>
								</View>
							)}
							ListEmptyComponent={<ThemedText>Nenhum endereço encontrado.</ThemedText>}
						/>
					</View>
				)}

				<Button
					onPress={() => router.push('/(tabs)/user/address/postalcode')}
					className="w-full"
				>
					<ThemedText>CADASTRAR ENDEREÇO</ThemedText>
				</Button>
			</ThemedView>
		</ThemedView>
	);
};

export default AddressScreen;
