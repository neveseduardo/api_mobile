import { DrawerItemList } from '@react-navigation/drawer';
import { Drawer } from 'expo-router/drawer';
import { ScrollView, View } from 'react-native';
import { GestureHandlerRootView } from 'react-native-gesture-handler';
import { SafeAreaProvider, SafeAreaView } from 'react-native-safe-area-context';
import AdminDrawerContent from '@/components/ui/AdminDrawerContent';
import { AuthProvider } from '@/contexts/AdminAuthenticationContext';

export default function Layout() {
	return (
		<AuthProvider>
			<SafeAreaProvider>
				<SafeAreaView className="flex flex-1">
					<GestureHandlerRootView style={{ flex: 1 }}>
						<Drawer
							drawerContent={(props) => (
								<SafeAreaView>
									<View className="flex flex-col gap-4 p-5">
										<AdminDrawerContent />

										<ScrollView>
											<DrawerItemList {...props} />
										</ScrollView>
									</View>
								</SafeAreaView>
							)}
						>
							<Drawer.Screen name="index" options={{ title: 'Início', drawerLabel: 'Início' }} />
							<Drawer.Screen name="agendamentos" options={{ title: 'Agendamentos', drawerLabel: 'Agendamentos' }} />
							<Drawer.Screen name="medicos" options={{ title: 'Médicos', drawerLabel: 'Médicos' }} />
							<Drawer.Screen name="unidades" options={{ headerShown: false, drawerLabel: 'Unidades médicas' }} />
							<Drawer.Screen name="usuarios" options={{ headerShown: false, drawerLabel: 'Usuários' }} />
							<Drawer.Screen name="administradores" options={{ headerShown: false, drawerLabel: 'Administradores' }} />
						</Drawer>
					</GestureHandlerRootView>
				</SafeAreaView>
			</SafeAreaProvider>
		</AuthProvider>
	);
}