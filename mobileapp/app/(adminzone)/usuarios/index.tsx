import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import { useRouter } from 'expo-router';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useCallback, useEffect, useState } from 'react';
import { HttpClient } from '@/services/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { ActivityIndicator, FlatList, RefreshControl, Text, View, Modal } from 'react-native';
import CardCrud from '@/components/ui/CardCrud';
import { UserService } from '@/services/UserService';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserService(client);

const UserListScreen = () => {
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
			console.error('Erro ao buscar dados:', err);
			setList([]);
			setError('Erro ao carregar dados.');
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
				console.error('Erro ao deletar item:', err);
				setError('Erro ao deletar item.');
			} finally {
				closeDeleteModal();
			}
		}
	};

	function handleEdit(item: any) {

		router.push({
			pathname: '/(adminzone)/usuarios/usuario',
			params: { ...item, editable: true },
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
						<CardCrud item={item} onDelete={openDeleteModal} onEdit={() => handleEdit(item)}>
							<Text className="text-lg font-semibold uppercase text-slate-600 dark:text-slate-100">{item.name}</Text>
							<Text className="text-sm font-semibold text-slate-600 dark:text-slate-100">{item.email}</Text>
							<Text className="text-sm font-semibold text-slate-600 dark:text-slate-100">{item.cpf}</Text>
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
							{selectedItem?.name} - {selectedItem?.email}
						</Text>
						<Text className="mb-4 text-lg text-slate-800 dark:text-slate-100">
							Tem certeza que deseja deletar este item?
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
				onPress={() => router.push('/(adminzone)/usuarios/usuario')}
				circular
				color="primary"
				className="!w-[50px] !h-[50px] absolute right-0 bottom-0 m-5 shadow"
			>
				<Ionicons size={25} name="add" color={'#FFFFFF'} />
			</Button>
		</ThemedView>
	);
};

export default UserListScreen;