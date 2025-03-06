import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import { useRouter } from 'expo-router';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useCallback, useEffect, useState } from 'react';
import { HttpClient } from '@/services/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { ActivityIndicator, FlatList, RefreshControl, Text, View, Modal } from 'react-native';
import { UnitService } from '@/services/UnitService';
import CardCrud from '../../../components/ui/CardCrud';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UnitService(client);

const AddressListScreen = () => {
	const router = useRouter();
	const [list, setList] = useState<any[]>([]);
	const [loading, setLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | null>(null);

	const [isModalVisible, setModalVisible] = useState(false);
	const [selectedItem, setSelectedItem] = useState<any>(null);

	const fetchAddresses = useCallback(async () => {
		try {
			setLoading(true);
			const { data: response } = await service.getAllFromAdminAsync();
			setList(response.data);
			setError(null);
		} catch (err) {
			console.error('Erro ao buscar endereços:', err);
			setList([]);
			setError('Erro ao carregar endereços.');
		} finally {
			setLoading(false);
		}

		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, []);

	const onRefresh = useCallback(() => {
		fetchAddresses();
	}, [fetchAddresses]);

	useEffect(() => {
		fetchAddresses();
	}, [fetchAddresses]);

	const openDeleteModal = (item: any) => {
		setSelectedItem(item);
		setModalVisible(true);
	};

	const closeDeleteModal = () => {
		setModalVisible(false);
		setSelectedItem(null);
	};

	const confirmDelete = async () => {
		if (selectedItem) {
			try {
				await service.deleteFromAdminAsync(selectedItem.id);
				fetchAddresses();
			} catch (err) {
				console.error('Erro ao deletar endereço:', err);
				setError('Erro ao deletar endereço.');
			} finally {
				closeDeleteModal();
			}
		}
	};

	function handleEdit(item: any) {
		const { address, ...rest } = item;
		const addressData = { ...address, ...rest, editable: true };

		router.push({
			pathname: '/(adminzone)/unidades/address',
			params: { ...addressData },
		});
	}

	return (
		<ThemedView className="relative flex-1 w-full p-5">
			<ThemedView className="flex flex-col flex-1 w-full gap-4">
				{loading && <ActivityIndicator size="large" color="#007BFF" />}

				{error && <ThemedText className="text-center">{error}</ThemedText>}

				<FlatList
					data={list}
					keyExtractor={(item) => item.id.toString()}
					renderItem={({ item }) => (
						<CardCrud item={item} onDelete={openDeleteModal} onEdit={handleEdit}>
							<Text className="text-lg font-semibold uppercase text-slate-600 dark:text-slate-100">{item.name}</Text>
							<Text className="text-sm font-semibold text-slate-600 dark:text-slate-100">{item.address.cep} - {item.address.logradouro}, {item.address.numero}</Text>
							<Text className="text-sm uppercase text-slate-600 dark:text-slate-100">{item.address.cidade} - {item.address.estado}</Text>
						</CardCrud>
					)}
					contentContainerStyle={{ paddingBottom: 50, width: '100%', gap: 10 }}
					refreshControl={
						<RefreshControl
							refreshing={loading}
							onRefresh={onRefresh}
							colors={['#007BFF']}
							progressBackgroundColor="#FFFFFF"
						/>
					}
				/>
			</ThemedView>

			<Modal
				visible={isModalVisible}
				transparent={true}
				animationType="slide"
				onRequestClose={closeDeleteModal}
			>
				<View className="items-center justify-center flex-1 bg-black/50">
					<View className="p-6 bg-white rounded-lg w-80 dark:bg-slate-800">
						<Text className="mb-4 text-lg font-semibold text-slate-800 dark:text-slate-100">
							{selectedItem?.cep} - {selectedItem?.logradouro}, {selectedItem?.numero}
						</Text>
						<Text className="mb-4 text-lg text-slate-800 dark:text-slate-100">
							Tem certeza que deseja deletar este endereço?
						</Text>
						<View className="grid grid-cols-2">
							<Button
								className="w-full"
								color="default"
								onPress={closeDeleteModal}
							>
								<Text className="text-sm font-semibold text-white uppercase">Cancelar</Text>
							</Button>
							<Button
								className="w-full"
								color="danger"
								onPress={confirmDelete}
							>
								<Text className="text-sm font-semibold text-white uppercase">Deletar</Text>
							</Button>
						</View>
					</View>
				</View>
			</Modal>

			<Button
				onPress={() => router.push('/(adminzone)/unidades/postalcode')}
				circular
				color="primary"
				className="!w-[50px] !h-[50px] absolute right-0 bottom-0 m-5 shadow"
			>
				<Ionicons size={25} name="add" color={'#FFFFFF'} />
			</Button>
		</ThemedView>
	);
};

export default AddressListScreen;