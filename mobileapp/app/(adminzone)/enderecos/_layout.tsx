import { Stack } from 'expo-router';
import { DrawerToggleButton } from '@react-navigation/drawer';
export default function AddressLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen name="index" options={{
				headerShown: true,
				title: 'Lista de endereços',
				headerLeft: ({ tintColor }) => (<DrawerToggleButton tintColor={tintColor} />),
			}}
			/>
			<Stack.Screen name="postalcode" options={{ headerShown: true, headerTitle: 'Buscar CEP' }} />
			<Stack.Screen name="address" options={{ headerShown: true, headerTitle: 'Cadastrar endereço' }} />
		</Stack>
	);
}