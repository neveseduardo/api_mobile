import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import { useRouter } from 'expo-router';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useCallback, useEffect, useState } from 'react';
import { HttpClient } from '@/services/restrict/HttpClient';
import { ActivityIndicator, FlatList, RefreshControl, Text } from 'react-native';
import CardCrud from '@/components/ui/CardCrud';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { UserAddressService } from '@/services/public/UserAddressService';
import { useActionSheet } from '@expo/react-native-action-sheet';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserAddressService(client);

export default function AddressListScreen() {
	const router = useRouter();
	const [list, setList] = useState<any[]>([]);
	const [loading, setLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | null>(null);
	const [selectedItem, setSelectedItem] = useState<any>(null);
	const { showActionSheetWithOptions } = useActionSheet();

	const fetchAddresses = useCallback(async () => {
		try {
			setLoading(true);
			const { data: response } = await service.getAllFromUserAsync();
			const enderecos = Array.isArray(response.data) ? null : response.data;

			setList(enderecos ? [enderecos] : []);
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

	const deleteItem = useCallback(async () => {
		try {
			await service.deleteFromUserAsync(selectedItem.id);
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

	function handleEdit(item: any) {
		const { address, ...rest } = item;
		const addressData = { ...address, ...rest, editable: true };

		router.push({
			pathname: '/(userzone)/perfil/enderecos/address',
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
					keyExtractor={(item) => item?.id.toString()}
					renderItem={({ item }) => (
						<CardCrud item={item} onDelete={onPressDelete} onEdit={handleEdit}>
							<Text className="text-sm font-semibold text-slate-600 dark:text-slate-100">{item.cep} - {item.logradouro}, {item.numero}</Text>
							<Text className="text-sm uppercase text-slate-600 dark:text-slate-100">{item.cidade} - {item.estado}</Text>
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
				onPress={() => router.push('/(userzone)/perfil/enderecos/postalcode')}
				circular
				color="primary"
				className="!w-[50px] !h-[50px] absolute right-0 bottom-0 m-5 shadow"
				disabled={!!list.length}
			>
				<Ionicons size={25} name="add" color={'#FFFFFF'} />
			</Button>
		</ThemedView>
	);
};