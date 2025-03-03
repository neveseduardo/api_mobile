import { Drawer } from 'expo-router/drawer';
import { GestureHandlerRootView } from 'react-native-gesture-handler';

export default function Layout() {
	return (
		<GestureHandlerRootView style={{ flex: 1 }}>

			<Drawer>
				<Drawer.Screen name="index" options={{ drawerLabel: 'Início', headerShown: false }} />
				<Drawer.Screen name="agendamentos" options={{ drawerLabel: 'Agendamentos', headerShown: false }} />
				<Drawer.Screen name="enderecos" options={{ drawerLabel: 'Endereços', headerShown: false }} />
				<Drawer.Screen name="medicos" options={{ drawerLabel: 'Médicos', headerShown: false }} />
				<Drawer.Screen name="unidades" options={{ drawerLabel: 'Unidades médicas', headerShown: false }} />
				<Drawer.Screen name="usuarios" options={{ drawerLabel: 'Usuários', headerShown: false }} />
				<Drawer.Screen name="administradores" options={{ drawerLabel: 'Administradores', headerShown: false }} />
			</Drawer>
		</GestureHandlerRootView>
	);
}