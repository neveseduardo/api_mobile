import { Drawer } from 'expo-router/drawer';
import { GestureHandlerRootView } from 'react-native-gesture-handler';
import { SafeAreaProvider, SafeAreaView } from 'react-native-safe-area-context';
import Button from '../../components/ui/Button';
import Ionicons from '@expo/vector-icons/Ionicons';

export default function Layout() {
	return (
		<SafeAreaProvider>
			<SafeAreaView className="flex flex-1">
				<GestureHandlerRootView style={{ flex: 1 }}>
					<Drawer>
						<Drawer.Screen name="index" options={{ title: 'Início', drawerLabel: 'Início' }} />
						<Drawer.Screen name="agendamentos" options={{ title: 'Agendamentos', drawerLabel: 'Agendamentos' }} />
						<Drawer.Screen name="enderecos" options={{ headerShown: false }} />
						<Drawer.Screen name="medicos" options={{ title: 'Médicos', drawerLabel: 'Médicos' }} />
						<Drawer.Screen name="unidades" options={{ title: 'Unidades médicas', drawerLabel: 'Unidades médicas' }} />
						<Drawer.Screen name="usuarios" options={{ title: 'Usuários', drawerLabel: 'Usuários' }} />
						<Drawer.Screen name="administradores" options={{ title: 'Administradores', drawerLabel: 'Administradores' }} />
					</Drawer>
				</GestureHandlerRootView>
			</SafeAreaView>
		</SafeAreaProvider>

	);
}