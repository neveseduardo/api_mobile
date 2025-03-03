import { Tabs } from 'expo-router';
import { Platform } from 'react-native';
import TabBarBackground from '@/components/ui/TabBarBackground';
import { AuthProvider } from '@/contexts/UserAuthenticationContext';
import Ionicons from '@expo/vector-icons/Ionicons';
import '@/assets/styles/global.css';
import { HapticTab } from '@/components/ui/HapticTab';

export default function TabLayout() {

	return (
		<AuthProvider>
			<Tabs
				screenOptions={{
					headerShown: false,
					tabBarButton: HapticTab,
					tabBarBackground: TabBarBackground,
					tabBarStyle: Platform.select({
						ios: { position: 'absolute' },
						default: {},
					}),
				}}
			>
				<Tabs.Screen
					name="index"
					options={{
						title: 'Home',
						tabBarIcon: ({ color }) => <Ionicons name="home-outline" size={20} color={color} />,
					}}
				/>
				<Tabs.Screen
					name="pesquisar"
					options={{
						title: 'Pesquisar',
						tabBarIcon: ({ color }) => <Ionicons name="search" size={20} color={color} />,
					}}
				/>
				<Tabs.Screen
					name="agendamentos"
					options={{
						title: 'Agendamentos',
						tabBarIcon: ({ color }) => <Ionicons name="calendar-clear-outline" size={20} color={color} />,
					}}
				/>
				<Tabs.Screen
					name="unidades"
					options={{
						title: 'Unidades',
						tabBarIcon: ({ color }) => <Ionicons name="business-outline" size={20} color={color} />,
					}}
				/>
				<Tabs.Screen
					name="perfil"
					options={{
						title: 'Perfil',
						tabBarIcon: ({ color }) => <Ionicons name="person-outline" size={20} color={color} />,
					}}
				/>
			</Tabs>
		</AuthProvider>
	);
}
