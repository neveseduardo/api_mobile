import { DrawerItemList } from '@react-navigation/drawer';
import { Drawer } from 'expo-router/drawer';
import { ScrollView, View } from 'react-native';
import { GestureHandlerRootView } from 'react-native-gesture-handler';
import { SafeAreaProvider, SafeAreaView } from 'react-native-safe-area-context';
import AdminDrawerContent from '@/components/ui/AdminDrawerContent';
import { AuthProvider } from '@/contexts/AdminAuthenticationContext';
import AdminLogoutButton from '../../components/ui/AdminLogoutButton';
import { Ionicons } from '@expo/vector-icons'; // Importe o Ionicons

export default function Layout() {
	return (
		<AuthProvider>
			<SafeAreaProvider>
				<SafeAreaView className="flex flex-1">
					<GestureHandlerRootView style={{ flex: 1 }}>
						<Drawer
							drawerContent={(props) => (
								<SafeAreaView className="flex-1">
									<View className="flex-1 gap-4 p-5">
										<AdminDrawerContent />

										<View className="flex-1">
											<ScrollView className="flex-1">
												<DrawerItemList {...props} />
											</ScrollView>
										</View>

										<AdminLogoutButton />
									</View>
								</SafeAreaView>
							)}
						>
							<Drawer.Screen
								name="index"
								options={{
									title: 'Início',
									drawerLabel: 'Início',
									drawerIcon: ({ color, size }) => (<Ionicons name="home-outline" size={size} color={color} />),
								}}
							/>
							<Drawer.Screen
								name="administradores"
								options={{
									headerShown: false,
									drawerLabel: 'Administradores',
									drawerIcon: ({ color, size }) => (<Ionicons name="people-outline" size={size} color={color} />),
								}}
							/>
							<Drawer.Screen
								name="usuarios"
								options={{
									headerShown: false,
									drawerLabel: 'Usuários',
									drawerIcon: ({ color, size }) => (<Ionicons name="person-outline" size={size} color={color} />),
								}}
							/>
							<Drawer.Screen
								name="agendamentos"
								options={{
									title: 'Agendamentos',
									drawerLabel: 'Agendamentos',
									drawerIcon: ({ color, size }) => (<Ionicons name="calendar-outline" size={size} color={color} />),
								}}
							/>
							<Drawer.Screen
								name="medicos"
								options={{
									title: 'Médicos',
									drawerLabel: 'Médicos',
									drawerIcon: ({ color, size }) => (<Ionicons name="medical-outline" size={size} color={color} />),
								}}
							/>
							<Drawer.Screen
								name="exames"
								options={{
									title: 'Exames',
									drawerLabel: 'Exames',
									drawerIcon: ({ color, size }) => (<Ionicons name="document-outline" size={size} color={color} />),
								}}
							/>
							<Drawer.Screen
								name="unidades"
								options={{
									headerShown: false,
									drawerLabel: 'Unidades médicas',
									drawerIcon: ({ color, size }) => (<Ionicons name="medkit-outline" size={size} color={color} />),
								}}
							/>
						</Drawer>
					</GestureHandlerRootView>
				</SafeAreaView>
			</SafeAreaProvider>
		</AuthProvider>
	);
}