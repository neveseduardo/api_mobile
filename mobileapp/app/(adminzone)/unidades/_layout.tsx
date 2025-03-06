import { Stack } from 'expo-router';
import { DrawerToggleButton } from '@react-navigation/drawer';
export default function AddressLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen name="index" options={{
				headerShown: true,
				title: 'Unidades de saúde',
				headerLeft: ({ tintColor }) => (<DrawerToggleButton tintColor={tintColor} />),
			}}
			/>
			<Stack.Screen name="postalcode" options={{ headerShown: true, headerTitle: 'Buscar CEP' }} />
			<Stack.Screen name="address" options={{ headerShown: true, headerTitle: 'Dados do endereço' }} />
			<Stack.Screen name="unidade" options={{ headerShown: true, headerTitle: 'Dados da unidade' }} />
		</Stack>
	);
}