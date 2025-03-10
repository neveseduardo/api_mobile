import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import { useRouter } from 'expo-router';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useCallback, useEffect, useState } from 'react';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { ActivityIndicator, FlatList, RefreshControl, Text } from 'react-native';
import CardCrud from '@/components/ui/CardCrud';
import { UserService } from '@/services/restrict/UserService';
import { useActionSheet } from '@expo/react-native-action-sheet';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserService(client);

const UserListScreen = () => {
	const router = useRouter();
	const [list, setList] = useState<any[]>([]);
	const [loading, setLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | null>(null);
	const [selectedItem, setSelectedItem] = useState<any>(null);
	const { showActionSheetWithOptions } = useActionSheet();

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

	function handleEdit(item: any) {

		router.push({
			pathname: '/(adminzone)/usuarios/usuario',
			params: { ...item, editable: true },
		});
	}

	const deleteItem = useCallback(async () => {
		try {
			await service.deleteFromAdminAsync(selectedItem.id);
			fetchAddresses();
		} catch (err) {
			console.error('Erro ao deletar item:', err);
			setError('Erro ao deletar item.');
		}
	}, [fetchAddresses, selectedItem]);

	const onPressDelete = useCallback((item: any) => {
		setSelectedItem(item);

		const options = ['Deletar', 'Cancelar'];
		const destructiveButtonIndex = 0;
		const cancelButtonIndex = 1;

		showActionSheetWithOptions({
			options,
			cancelButtonIndex,
			destructiveButtonIndex,
		}, (selected) => {
			if (selected === destructiveButtonIndex) {
				deleteItem();
			}
		});
	}, [deleteItem, showActionSheetWithOptions]);

	return (
		<ThemedView className="relative flex-1 w-full p-5">
			<ThemedView className="flex flex-col flex-1 w-full gap-4">
				{loading && <ActivityIndicator size="large" color="#007BFF" />}

				{error && <ThemedText className="text-center">{error}</ThemedText>}

				<FlatList
					data={list}
					keyExtractor={(item) => item.id.toString()}
					renderItem={({ item }) => (
						<CardCrud item={item} onDelete={onPressDelete} onEdit={() => handleEdit(item)}>
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