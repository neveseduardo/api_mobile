import { DrawerToggleButton } from '@react-navigation/drawer';
import { Stack } from 'expo-router';

export default function RootLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen name="index" options={{
				headerShown: true,
				title: 'Lista de usuários',
				headerLeft: ({ tintColor }) => (<DrawerToggleButton tintColor={tintColor} />),
			}}
			/>
			<Stack.Screen name="usuario" options={{ title: 'Formulário usuário' }} />
		</Stack>
	);
}