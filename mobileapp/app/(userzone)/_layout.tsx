import { Tabs } from 'expo-router';
import TabBarBackground from '@/components/ui/TabBarBackground';
import { AuthProvider } from '@/contexts/UserAuthenticationContext';
import Ionicons from '@expo/vector-icons/Ionicons';
import { HapticTab } from '@/components/ui/HapticTab';
import { Platform } from 'react-native';

export default function TabLayout() {

	return (
		<AuthProvider>
			<Tabs
				screenOptions={{
					headerShown: false,
					tabBarButton: HapticTab,
					tabBarBackground: TabBarBackground,
					// tabBarStyle: Platform.select({
					// 	ios: { position: 'absolute' },
					// 	default: {},
					// }),
				}}
			>
				<Tabs.Screen
					name="index"
					options={{
						title: 'Home',
						tabBarIcon: ({ color, size }) => <Ionicons name="home-outline" size={size} color={color} />,
					}}
				/>

				<Tabs.Screen
					name="agendamentos"
					options={{
						title: 'Agendamentos',
						tabBarIcon: ({ color, size }) => <Ionicons name="calendar-clear-outline" size={size} color={color} />,
					}}
				/>

				<Tabs.Screen
					name="servicos"
					options={{
						title: 'Serviços',
						tabBarIcon: ({ color, size }) => <Ionicons name="business-outline" size={size} color={color} />,
					}}
				/>

				<Tabs.Screen
					name="perfil"
					options={{
						title: 'Perfil',
						tabBarIcon: ({ color, size }) => <Ionicons name="person-outline" size={size} color={color} />,
					}}
				/>
			</Tabs>
		</AuthProvider>
	);
}
