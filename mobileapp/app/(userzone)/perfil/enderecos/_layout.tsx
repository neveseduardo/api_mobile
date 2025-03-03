import { Stack } from 'expo-router';

export default function AddressLayout() {

	return (
		<Stack initialRouteName="index">
			<Stack.Screen name="index" options={{ headerShown: true, title: 'Meus endereços' }} />
			<Stack.Screen name="postalcode" options={{ headerShown: true, headerTitle: 'Buscar CEP' }} />
			<Stack.Screen name="address" options={{ headerShown: true, headerTitle: 'Cadastrar endereço' }} />
		</Stack>
	);
}