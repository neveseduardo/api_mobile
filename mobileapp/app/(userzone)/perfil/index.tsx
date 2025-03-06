import { ThemedView } from '@/components/ui/ThemedView';
import UserProfileHeader from '@/components/modules/user/UserProfileHeader';
import { useUserAuth } from '@/contexts/UserAuthenticationContext';
import Button from '@/components/ui/Button';
import { Text, View } from 'react-native';
import { router } from 'expo-router';
import UserLogoutButton from '@/components/ui/UserLogoutButton';
import { SafeAreaView } from 'react-native-safe-area-context';

const UserScreen = () => {
	const { userData } = useUserAuth();

	return (
		<SafeAreaView className="flex-1">
			<ThemedView className="flex-1 w-full gap-5 p-5">
				<UserProfileHeader username={userData?.name} email={userData?.email} />

				<View className="flex-1">

					<Button className="w-full" color="primary" onPress={() => router.push('/(userzone)/perfil/enderecos')}>
						<Text className="text-white">ENDEREÇOS</Text>
					</Button>
				</View>

				<UserLogoutButton />
			</ThemedView>
		</SafeAreaView>
	);
};

export default UserScreen;